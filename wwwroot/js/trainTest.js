$(document).ready(function () {
    let answers =  [];
    let csrfToken = $('input[name="__RequestVerificationToken"]').val();
    let subtopicId = $('#questions-container').data('subtopic-id');

    function loadAnsweredQuestions() {
        answers.forEach(answer => {
            let questionContainer = $(`.question-container[data-question-id="${answer.questionId}"]`);
            let selectedAnswer = questionContainer.find(`input[value="${answer.selectedAnswerId}"]`);
            if (answer.isCorrect) {
                selectedAnswer.closest('label').addClass('correct-answer');
            } else {
                selectedAnswer.closest('label').addClass('wrong-answer');
            }
        });
    }

    function blockAnsweredQuestion() {
        let currentQuestionId = $('.question-container').data('question-id');
        let answeredQuestion = answers.find(answer => answer.questionId == currentQuestionId);

        if (answeredQuestion) {
            $('input[name="answer-' + currentQuestionId + '"]').prop('disabled', true);
            $('#answer-button').prop('disabled', true);
            $('#next-question-button').prop('disabled', false); // Enable "Next Question" button
        } else {
            $('input[name="answer-' + currentQuestionId + '"]').prop('disabled', false);
            $('#answer-button').prop('disabled', false);
            $('#next-question-button').prop('disabled', true); // Disable "Next Question" button
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
                    selectedAnswer.closest('label').addClass('correct-answer');
                } else {
                    pageButton.removeClass('btn-default').addClass('btn-danger');
                    selectedAnswer.closest('label').addClass('wrong-answer');
                }

                answers.push({ questionId: questionId, selectedAnswerId: selectedAnswerId, isCorrect: data.isCorrect, pageIndex: pageIndex });
                localStorage.setItem('answers', JSON.stringify(answers));

                blockAnsweredQuestion(); // Block the current question and enable the "Next Question" button
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    });

    $('#next-question-button').click(function () {
        let pageIndex = $('#questions-container').data('current-page');

        $.ajax({
            url: '/Train/TrainTest/GetNextQuestion',
            type: 'POST',
            headers: {
                'X-CSRF-TOKEN': csrfToken
            },
            data: { currentPage: pageIndex, subtopicId: subtopicId },
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
    });

    $('#finish-button').click(function () {
        finishTest();
    });

    $('#report-question-button').click(function () {
        $('#reportQuestionModal').modal('show');
    });

    $('#submit-report-button').click(function () {
        let reportText = $('#report-text').val();
        let questionId = $('.question-container').data('question-id');

        if (!reportText.trim()) {
            alert('Пожалуйста, введите текст отчета!');
            return;
        }

        $.ajax({
            url: '/Train/TrainTest/ReportQuestion',
            type: 'POST',
            headers: {
                'X-CSRF-TOKEN': csrfToken
            },
            data: JSON.stringify({ questionId: questionId, text: reportText }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                alert('Ваш отчет был отправлен!');
                $('#reportQuestionModal').modal('hide');
                $('#report-text').val('');
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
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
            success: function () {
                window.location.href = `/Train/Subject/Index`;
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    }

    loadAnsweredQuestions();
    blockAnsweredQuestion();
});
