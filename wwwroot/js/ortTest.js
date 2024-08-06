document.addEventListener("DOMContentLoaded", function () {
    const testContainers = document.querySelectorAll('.test-container');
    const countdownElement = document.getElementById('countdown');
    const finishTestName = document.getElementById('finishTestName');
    let timers = {};
    let timeSpent = [];
    let selectedAnswers = {};
    let currentPage = 1;
    let totalTestTime = 0;
    let selectionCounts = {};

    function initializeTimers() {
        totalTestTime = 0;
        testContainers.forEach(testContainer => {
            const testId = testContainer.dataset.testid;
            const testTime = parseInt(testContainer.dataset.testtime, 10) * 60;
            totalTestTime += testTime;
            timers[testId] = {
                originalTime: testTime,
                timeLeft: testTime,
                interval: null
            };
        });
        updateTotalCountdown();
    }

    function formatTime(seconds) {
        const hours = Math.floor(seconds / 3600);
        const minutes = Math.floor((seconds % 3600) / 60);
        const secs = seconds % 60;
        return `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:${String(secs).padStart(2, '0')}`;
    }

    function startTimer(testId) {
        if (timers[testId].interval) {
            clearInterval(timers[testId].interval);
        }
        timers[testId].interval = setInterval(function () {
            if (timers[testId].timeLeft <= 0) {
                clearInterval(timers[testId].interval);
                countdownElement.innerHTML = "Время вышло!";
            } else {
                timers[testId].timeLeft--;
                updateCountdown(testId);
                updateTotalCountdown();
            }
        }, 1000);
    }

    function stopTimer(testId) {
        if (timers[testId].interval) {
            clearInterval(timers[testId].interval);
        }
    }

    function saveTimeSpent(testId) {
        const timer = timers[testId];
        const timeSpentTest = (timer.originalTime - timer.timeLeft);
        const existingEntry = timeSpent.find(entry => entry.testId === testId);
        if (existingEntry) {
            existingEntry.timeSpent = timeSpentTest;
        } else {
            timeSpent.push({ testId: testId, timeSpent: timeSpentTest });
        }
    }

    function updateCountdown(testId) {
        const testContainer = document.querySelector(`.test-container[data-testid="${testId}"]`);
        const testName = testContainer.dataset.testname;
        const timeLeft = timers[testId].timeLeft;
        countdownElement.innerHTML = `${formatTime(timeLeft)} <br> <span> ${testName} </span>`;
    }

    function updateTotalCountdown() {
        const totalTimeLeft = Object.values(timers).reduce((acc, timer) => acc + timer.timeLeft, 0);
        document.getElementById('total-countdown').innerHTML = `${formatTime(totalTimeLeft)} <br> <span>Общее время</span>`;
    }

    function updatePaginationButtonState() {
        document.getElementById('prev-page').disabled = currentPage === 1;
        const currentTestContainer = testContainers[currentPage - 1];
        const testNameForButton = currentTestContainer.dataset.testname;
        document.getElementById('next-page').textContent = 'Завершить ' + testNameForButton;
    }

    document.getElementById('prev-page').addEventListener('click', function () {
        if (currentPage > 1) {
            const currentTestId = testContainers[currentPage - 1].dataset.testid;
            stopTimer(currentTestId);
            currentPage--;
            renderTests();
        }
    });

    document.getElementById('next-page').addEventListener('click', function () {
        const totalTests = testContainers.length;
        const currentTestId = testContainers[currentPage - 1].dataset.testid;
        saveTimeSpent(currentTestId);
        stopTimer(currentTestId);

        if (currentPage < totalTests) {
            currentPage++;
            renderTests();
        } else {
            axios.post('@Url.Action("Index", "Tests")', {
                selectedAnswers: Object.values(selectedAnswers),
                timeSpent: timeSpent,
                ortTestId: '@ViewBag.OrtTestId'
            })
                .then(response => {
                    const ortTestResultModels = response.data.ortTestResultModels;
                    const queryParams = new URLSearchParams();
                    ortTestResultModels.forEach((model, index) => {
                        queryParams.append(`ortTestResultModels[${index}].rightsCount`, model.rightsCount);
                        queryParams.append(`ortTestResultModels[${index}].questionCount`, model.questionCount);
                        queryParams.append(`ortTestResultModels[${index}].testId`, model.testId);
                        queryParams.append(`ortTestResultModels[${index}].spentTimeInMin`, model.spentTimeInMin);
                        queryParams.append(`ortTestResultModels[${index}].testPoints`, model.testPoints);
                        queryParams.append(`ortTestResultModels[${index}].pointsSum`, model.pointsSum);
                    });
                    window.location.href = `@Url.Action("Result", "Tests")?${queryParams.toString()}`;
                })
                .catch(error => {
                    console.error('Error sending data:', error);
                });
        }
    });

    function handleOffcanvasRadioClick(event) {
        const radio = event.target;
        const questionId = radio.name.split('-')[1];
        const testContainer = document.querySelector(`[data-questionid="${questionId}"]`).closest('.test-container');
        const testId = testContainer.dataset.testid;
        const questionContainer = radio.closest('.question-container');

        if (radio.checked) {
            selectedAnswers[radio.name] = {
                testId: testId,
                questionId: questionId,
                answerId: radio.value
            };
            selectionCounts[questionId] = (selectionCounts[questionId] || 0) + 1;
        } else {
            delete selectedAnswers[radio.name];
            selectionCounts[questionId] = (selectionCounts[questionId] || 0) - 1;
        }

        if (selectionCounts[questionId] >= 2) {
            document.querySelectorAll(`[data-questionid="${questionId}"] .answer-radio`).forEach(function (radio) {
                if (!radio.checked) {
                    radio.disabled = true;
                }
            });
        } else {
            document.querySelectorAll(`[data-questionid="${questionId}"] .answer-radio`).forEach(function (radio) {
                radio.disabled = false;
            });
        }
    }

    document.querySelectorAll('.offcanvas .answer-radio').forEach(function (radio) {
        radio.addEventListener('click', handleOffcanvasRadioClick);
    });

    initializeTimers();
    renderTests();

    function renderTests() {
        window.scrollTo(0, 0);
        testContainers.forEach(function (testContainer, index) {
            testContainer.style.display = (index === currentPage - 1) ? 'block' : 'none';
        });
        const testId = testContainers[currentPage - 1].dataset.testid;
        startTimer(testId);
        updatePaginationButtonState();
    }
});