import axios, { AxiosResponse } from 'axios';

export class ApiClient {
    private _token: string = "";
    private _axiosClient = axios.create()

    updateToken() {
        this._token = this.fetchToken();
    }

    fetchToken(email: string = '', password: string = '') {
        if (email === '' || password === '')
            return ''
        
        let t = 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMzdmNTYwNzYtNGVmZi00MDBkLThiYTAtMGM4YjM3NDkxMTgyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiTWVtYmVyIiwibmJmIjoxNTgzMTYzMTU3LCJleHAiOjE4OTQyMDMxNTcsImlzcyI6IlNvY2lhbE5ldHdvcmsuQXV0aG9yaXphdGlvblNlcnZlciIsImF1ZCI6IlNvY2lhbE5ldHdvcmsuV2ViQ2xpZW50In0.WTjjZYp4JbEkgCHO9TV8n4aTJWJfAO9RWgrgIK9Bj78'
        localStorage.setItem('token', t)
        return t ? t : ''
    }

    post() {
        
    }
}