$(document).ready(function () {
    let answers = [];
    let csrfToken = $('input[name="__RequestVerificationToken"]').val();
    let subtopicId = $('#questions-container').data('subtopic-id');

    $('#answer-button').click(function () {
        let questionId = $('.question-container').data('question-id');
        let selectedAnswer = $('input[name="answer-' + questionId + '"]:checked');
        console.log('Current Question ID:', questionId);
        console.log('Selected Answer:', selectedAnswer);
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
                console.log('CheckAnswer success:', data);
                let pageIndex = $('#questions-container').data('current-page');
                console.log('Page Index: ', pageIndex);
                console.log('Subtopic ID: ', subtopicId);
                let pageButton = $('.pagination-btn').eq(pageIndex - 1);
                console.log('Page Button: ', pageButton);

                if (data.isCorrect) {
                    pageButton.removeClass('btn-default').addClass('btn-success');
                } else {
                    pageButton.removeClass('btn-default').addClass('btn-danger');
                }

                answers.push({ questionId: questionId, selectedAnswerId: selectedAnswerId });

                $.ajax({
                    url: '/Train/TrainTest/GetNextQuestion',
                    type: 'POST',
                    headers: {
                        'X-CSRF-TOKEN': csrfToken
                    },
                    data: { currentPage: pageIndex, subtopicId: subtopicId },
                    success: function (response) {
                        if (response.finished) {
                            finishTest();
                        } else {
                            $('#questions-container').html(response);
                            $('#questions-container').data('current-page', pageIndex + 1);
                            var newQuestionId = $('.question-container').data('question-id');
                            $('#questions-container').attr('data-question-id', newQuestionId);
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
        alert('Нелинейная навигация не разрешена!');
    });
});