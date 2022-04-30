import http from 'k6/http';

export let options = {
    stages: [
        { duration: '20m', target: 1 },
        { duration: '20m', target: 10 },
        { duration: '20m', target: 20 },
    ],
};

export default function () {
    http.get('http://35.188.223.237/Match/slow/100/800');
    http.get('http://35.188.223.237/Match/slow/100/900');
    http.get('http://35.188.223.237/Match/slow/100/1000');
    http.get('http://35.188.223.237/Match/fast/10');
    http.get('http://35.188.223.237/Match/fast/9');
    http.get('http://35.188.223.237/Match/fast/8');
    http.get('http://35.188.223.237/Match/fast/7');
    http.get('http://35.188.223.237/Match/fast/6');
    http.get('http://35.188.223.237/Match/fast/5');
    http.get('http://35.188.223.237/Match/fast/3');
    http.get('http://35.188.223.237/Match/fast/2');
    http.get('http://35.188.223.237/Match/fast/1');
    http.get('http://35.188.223.237/Match/roulette');
}