import http from 'k6/http';

export let options = {
    stages: [
        { duration: '20m', target: 1 },
        { duration: '20m', target: 10 },
        { duration: '20m', target: 20 },
    ],
};

export default function () {
    http.get('http://35.188.223.237/Match/slow/100/200');
    http.get('http://35.188.223.237/Match/slow/100/400');
    http.get('http://35.188.223.237/Match/slow/100/500');
    http.get('http://35.188.223.237/Match/fast/100');
    http.get('http://35.188.223.237/Match/fast/90');
    http.get('http://35.188.223.237/Match/fast/80');
    http.get('http://35.188.223.237/Match/fast/70');
    http.get('http://35.188.223.237/Match/fast/60');
    http.get('http://35.188.223.237/Match/fast/50');
    http.get('http://35.188.223.237/Match/fast/30');
    http.get('http://35.188.223.237/Match/fast/20');
    http.get('http://35.188.223.237/Match/fast/10');
    http.get('http://35.188.223.237/Match/roulette');
}