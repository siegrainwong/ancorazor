import { AxiosResponse, AxiosRequestConfig } from 'axios';
import axios from 'axios';

const IsDevelopment: boolean = true;
axios.defaults.headers = {
    //'X-Requested-With': 'XMLHttpRequest',
    'Content-Type': 'application/x-www-form-urlencoded',
};
axios.defaults.baseURL = IsDevelopment ? "https://localhost:5001/api" : "shit";
axios.defaults.timeout = 20000;

const enum Methods {
    GET = 'GET',
    POST = 'POST',
    PUT = 'PUT',
    DELETE = 'DELETE'
}

export default {
    async get(url: string, query?: any, option?: AxiosRequestConfig) {
        return await handleRequest(Methods.GET, url, null, query, option);
    },
    async post(url: string, body: any, query?: any, option?: AxiosRequestConfig) {
        return await handleRequest(Methods.POST, url, body, query, option);
    },
    async put(url: string, body?: any, query?: any, option?: AxiosRequestConfig) {
        return await handleRequest(Methods.PUT, url, body, query, option);
    },
    async delete(url: string, query?: any, option?: AxiosRequestConfig) {
        return await handleRequest(Methods.DELETE, url, null, query, option);
    },
}

async function handleRequest(method: Methods, url: string, body?: any, query?: any, option?: AxiosRequestConfig) {
    const res = await axios.request({
        method: method.toString(),
        url: url,
        params: query,
        data: body,
        withCredentials: false
    })
    return handleResponse(res)
}

/**
 * 响应处理
 * @param res 
 */
function handleResponse(res: AxiosResponse): { succeed: boolean, data: any } {
    let result = null;
    switch (res.status) {
        case 200:
            result = { succeed: true, data: res.data }
            break;
        case 302:
            result = { succeed: false, data: `重定向到` + res.data }
            break;
        case 400:
            // 如果服务器返回错误信息,就显示服务器的信息,否则显示请求错误
            result = { succeed: false, data: res.data ? res.data : '请求错误' }
            break;
        case 401:
            result = { succeed: false, data: res.data ? res.data : '请求要求用户的身份认证' }
            break;
        case 404:
            result = { succeed: false, data: res.data ? res.data : '不存在的资源' }
            break;
        case 413:
            result = { succeed: false, data: res.data ? res.data : '上传的资源体积过大' }
            break;
        case 500:
            result = { succeed: false, data: res.data ? res.data : '服务器内部错误，无法完成请求' }
            break;
        case 501:
            result = { succeed: false, data: res.data ? res.data : '服务器不支持请求的功能，无法完成请求' }
            break;
        default:
            result = { succeed: false, data: res.data ? res.data : '未分类的错误, status' + res.status }
            break;
    }
    function handleError(result: { succeed: boolean, data: any }) {
        if (result.succeed) {
            return { succeed: result.succeed, data: result.data }
        } else {
            showError(result.data)
            return { succeed: result.succeed, data: result.data }
        }
    }
    function showError(msg: string) {
        if (IsDevelopment) {
            alert(msg)
        } else {
            console.log(msg);
        }
    }
    return handleError(result);
}