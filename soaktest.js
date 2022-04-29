import http from 'k6/http';

export let options = {
    stages: [
        { duration: '60s', target: 1 },
        { duration: '60s', target: 10 },
        { duration: '60s', target: 20 },
    ],
};

export default function () {
    http.get('http://35.188.223.237/Match/slow/100/50');
    http.get('http://35.188.223.237/Match/fast/100');
    http.get('http://35.188.223.237/Match/roulette');
}
}