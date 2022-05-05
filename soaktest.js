import http from 'k6/http';

export let options = {
    stages: [
        { duration: '5m', target: 1 },
        { duration: '5m', target: 10 },
        { duration: '5m', target: 20 },
    ],
};

export default function () {
    http.get('http://35.188.223.237/Match/slow/500');
    http.get('http://35.188.223.237/Match/slow/400');
    http.get('http://35.188.223.237/Match/slow/300');
    http.get('http://35.188.223.237/Match/fast/20');
    http.get('http://35.188.223.237/Match/fast/10');
    http.get('http://35.188.223.237/Match/fast/5');
    http.get('http://35.188.223.237/Match/fast/1');
    http.get('http://35.188.223.237/Match/roulette');
}