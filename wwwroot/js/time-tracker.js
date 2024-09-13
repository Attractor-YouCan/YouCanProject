function trackTime() {
    let startTime = Date.now();

    function logTime() {
        let endTime = Date.now();
        let timeSpent = endTime - startTime;

        fetch('/Account/LogTime', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(timeSpent),  // Передаем число, а не объект
            keepalive: true
        }).then(response => {
            if (!response.ok) {
                console.error('Failed to log time:', response.statusText);
            }
        }).catch(error => {
            console.error('Error logging time:', error);
        });

        // Сбрасываем таймер для следующей сессии
        startTime = Date.now();
    }

    // Отправляем данные каждые 60 секунд
    setInterval(logTime, 60000);

    // Отправляем данные при закрытии страницы
    window.addEventListener("beforeunload", logTime);
}