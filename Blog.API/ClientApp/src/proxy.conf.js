/**
 * Knowledge: Angular 代理
 * 知道这么用就行了，它用这个是为了避免每次用httpClient请求时拼接Url
 * 我们用axios没有这个麻烦
 */
const PROXY_CONFIG = [
    // {
    //     context: [
    //         "/api"
    //     ],
    //     target: "https://localhost:5001",
    //     secure: true
    // }
];

module.exports = PROXY_CONFIG;