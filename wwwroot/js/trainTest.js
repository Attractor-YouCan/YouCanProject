$(document).ready(function () {
    let answers = [];
    let csrfToken = $('input[name="__RequestVerificationToken"]').val();
    let subtopicId = $('#questions-container').data('subtopic-id');

    function loadAnsweredQuestions() {
        answers.forEach(answer => {
            let pageButton = $('.pagination-btn').eq(answer.pageIndex - 1);
            if (answer.isCorrect) {
                pageButton.removeClass('btn-default').addClass('btn-success');
            } else {
                pageButton.removeClass('btn-default').addClass('btn-danger');
            }
        });
    }

    function blockAnsweredQuestion() {
        let currentQuestionId = $('.question-container').data('question-id');
        let answeredQuestion = answers.find(answer => answer.questionId == currentQuestionId);

        if (answeredQuestion) {
            $('input[name="answer-' + currentQuestionId + '"]').prop('disabled', true);
            $('#answer-button').prop('disabled', true);
        } else {
            $('input[name="answer-' + currentQuestionId + '"]').prop('disabled', false);
            $('#answer-button').prop('disabled', false);
        }
    }

    $('#answer-button').click(function () {
        let questionId = $('.question-container').data('question-id');
        let selectedAnswer = $('input[name="answer-' + questionId + '"]:checked');
        if (!selectedAnswer.length) {
            alert('Пожалуйста, выберите ответ!');
            return;
        }

        let selectedAnswerId = selectedAnswer.val();

        $.ajax({
            url: '/Train/TrainTest/CheckAnswer',
            type: 'POST',
            headers: {
                'X-CSRF-TOKEN': csrfToken
            },
            data: JSON.stringify({ questionId: questionId, selectedAnswerId: selectedAnswerId }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                let pageIndex = $('#questions-container').data('current-page');
                let pageButton = $('.pagination-btn').eq(pageIndex - 1);

                if (data.isCorrect) {
                    pageButton.removeClass('btn-default').addClass('btn-success');
                } else {
                    pageButton.removeClass('btn-default').addClass('btn-danger');
                }

                answers.push({ questionId: questionId, selectedAnswerId: selectedAnswerId, isCorrect: data.isCorrect, pageIndex: pageIndex });
                localStorage.setItem('answers', JSON.stringify(answers));

                $.ajax({
                    url: '/Train/TrainTest/GetNextQuestion',
                    type: 'POST',
                    headers: {
                        'X-CSRF-TOKEN': csrfToken
                    },
                    data: { currentPage: pageIndex, wantedPage: 0, subtopicId: subtopicId },
                    success: function (response) {
                        if (response.finished) {
                            blockAnsweredQuestion();
                            finishTest();
                        } else {
                            $('#questions-container').html(response);
                            $('#questions-container').data('current-page', pageIndex + 1);
                            blockAnsweredQuestion();
                        }
                    },
                    error: function (error) {
                        console.error('Error:', error);
                    }
                });
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    });

    $('#finish-button').click(function () {
        finishTest();
    });

    function finishTest() {
        $.ajax({
            url: '/Train/TrainTest/FinishTest',
            type: 'POST',
            headers: {
                'X-CSRF-TOKEN': csrfToken
            },
            data: JSON.stringify({ answers: answers }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                const resultModelJson = encodeURIComponent(JSON.stringify(data.resultModel));
                window.location.href = `/Train/TrainTest/TestResult?resultModel=${resultModelJson}`;
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    }

    $('.pagination-btn').click(function (e) {
        e.preventDefault();
        let wantedPage = parseInt($(this).text());
        $.ajax({
            url: '/Train/TrainTest/GetNextQuestion',
            type: 'POST',
            headers: {
                'X-CSRF-TOKEN': csrfToken
            },
            data: { currentPage: 0, wantedPage: wantedPage, subtopicId: subtopicId },
            success: function (response) {
                $('#questions-container').html(response);
                $('#questions-container').data('current-page', wantedPage);
                blockAnsweredQuestion();
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    });

    loadAnsweredQuestions();
    blockAnsweredQuestion();
});
