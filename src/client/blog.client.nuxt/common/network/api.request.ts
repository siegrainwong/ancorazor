import { AxiosResponse, AxiosRequestConfig } from 'axios';
import axios from 'axios';
// import router from 'router'

import { getModule } from "vuex-module-decorators";
import store from "../../store/store";
import { UserModule } from "../stores/userModule";

const IsDevelopment: boolean = true;
axios.defaults.headers = {
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
/**
 * API请求入口
 */
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

// const module: UserModule = getModule(UserModule, store)
// /**
//  * 鉴权拦截器
//  */
// axios.interceptors.request.use(
//     config => {
//         // 判断是否存在token，如果存在的话，则每个http header都加上token
//         if (module.isTokenValid) config.headers.Authorization = module.authToken;
//         return config;
//     },
//     err => {
//         return Promise.reject(err);
//     }
// );

// axios.interceptors.response.use(
//     response => {
//         return response;
//     },
//     error => {
//         const router = this.$router;
//         if (error.response) {
//             switch (error.response.status) {
//                 case 401:
//                     // 返回 401 清除token信息并跳转到登录页面
//                     router.replace({
//                         path: "login",
//                         query: { redirect: router.currentRoute.fullPath }
//                     });
//             }
//         }
//         return Promise.reject(error.response.data);
//     }
// );

/**
 * 请求处理
 * @param method Method
 * @param url URL
 * @param body Payload
 * @param query QueryString
 * @param option 选项
 */
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
 * @param response 
 */
function handleResponse(response: AxiosResponse): ResponseModel {
    let result = null;
    switch (response.status) {
        case 200:
            result = new ResponseModel(response.data.data, response.data.succeed)
            break;
        default:
            return new ResponseModel(response.data ? response.data : 'Request failed : status' + response.status)
    }

    if (result.succeed) {
        return new ResponseModel(result.data, true)
    } else {
        console.log(result.data);
        return new ResponseModel(result)
    }
}

class ResponseModel {
    succeed: boolean = false
    data?: any

    constructor(data?: any, succeed: boolean = false) {
        this.data = data;
        this.succeed = succeed;
    }
}