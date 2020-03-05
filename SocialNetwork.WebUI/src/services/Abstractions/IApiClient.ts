import { AxiosResponse, AxiosInstance, AxiosRequestConfig } from 'axios';
import { IAuthModel } from '@/models/responses/AuthModel';

export interface IApiClient {
    updateToken(): Promise<void>;
    fetchToken(email: string | null, password: string | null): Promise<AxiosResponse<IAuthModel>>;
    get<T = any, R = AxiosResponse<T>>(url: string, config?: AxiosRequestConfig ): Promise<R>;
    post<T = any, R = AxiosResponse<T>>(url: string, data?: any, config?: AxiosRequestConfig): Promise<R>;
}