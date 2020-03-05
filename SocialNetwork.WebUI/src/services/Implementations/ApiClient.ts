import axios, { AxiosResponse, AxiosInstance, AxiosRequestConfig } from 'axios';
import { IAuthModel } from '@/models/responses/AuthModel';

class ApiClient {
    private _axiosClient: AxiosInstance;

    constructor() {
        this._axiosClient = axios.create({
            baseURL: 'http://16ch.tk/api/'
        })
        this._axiosClient.interceptors.response.use((response) => this.authTokenExpiredInterceptor(response))
    }

    private authTokenExpiredInterceptor(response: AxiosResponse<any>): AxiosResponse<any> {
        if (response.status === 401)
            console.log('unathorized')

        return response;
    }

    async updateToken(): Promise<void> {
        this.fetchToken(localStorage.getItem('email'), localStorage.getItem('password'))
            .then(x => {
                if (x.status === 200)
                {
                    localStorage.setItem('token', x.data.access_token);
                    this._axiosClient.defaults.headers['Authorization'] = x.data.access_token;
                }
            })
            .catch(x => {
                console.log(x);
            })
    }

    async fetchToken(email: string | null = '', password: string | null = ''): Promise<AxiosResponse<IAuthModel>> {
        return await this._axiosClient.get<IAuthModel>('auth/Authorization', {
            params: {
                email: email ? email : 'anonim',
                password: password ? password : 'WERDwerd2012',
            }
        })
    }


    async get<T = any, R = AxiosResponse<T>>(url: string, config?: AxiosRequestConfig): Promise<R> {
        return this._axiosClient.get<T, R>(url, config);
    }

    async post<T = any, R = AxiosResponse<T>>(url: string, data?: any, config?: AxiosRequestConfig): Promise<R> {
        return this._axiosClient.post<T, R>(url, data, config);
    }
}

export default new ApiClient();