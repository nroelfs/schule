import axios, { AxiosInstance, AxiosResponse } from 'axios';
import authService from './auth.service';
class ApiService {
    private api: AxiosInstance;

    constructor(controller: string) {
        const envBaseURL = import.meta.env.VITE_API_URL;
        this.api = axios.create({
            baseURL: `${envBaseURL}/api/${controller}`,
        });
        const token = localStorage.getItem('accesstoken');
        if (!this.isHeaderSet() && token) {
            this.setHeader(token);
        }
        this.api.interceptors.request.use((config) => {
            const token = localStorage.getItem('accesstoken');
            if (token) {
                config.headers.Authorization = `Bearer ${token}`;
                config.headers['Content-Type'] = 'application/json';
            }
            return config;
        });
        this.api.interceptors.response.use(
            (response) => response,
            (error) => {
                if (error.response && error.response.status === 401) {
                    this.handle401();
                }
                return Promise.reject(error);
            }
        );
    }
    /**
     * 
     * @param accessToken : Setzt den Accesstoken der Zugriff auf weitere Methoden bietet
     */
    setHeader(accessToken:string ) {
        if(accessToken != null) axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
    }
    /**
     * 
     * @returns Bool : ob bereits ei n Accesstoken für den HTTP Header gesetzt wurde
     */
    isHeaderSet() {
        return axios.defaults.headers.common.hasOwnProperty('Authorization');
    }
    /**
     * Entfernt den Accesstoken aus dem Header
     */
    unsetHeader() {
        delete axios.defaults.headers.common['Authorization'];
    }
    /**
     * 
     * @param url endpunkt der API 
     * @returns Antwort der API
     */
    async get<T>(url: string): Promise<T> {
        const response: AxiosResponse<T> = await this.api.get(url);
        return response.data;
    }

    /**
     * 
     * @param url endpunkt der API 
     * @param data Daten die mittels POST übertragen werden sollen
     * @returns Antwort der API
     */
    async post<T ,D>(url: string, data: D): Promise<T> {
        const response: AxiosResponse<T> = await this.api.post(url, data);
        return response.data;
    }
    async handle401(){
        this.unsetHeader();
        try{
            const response = await authService.refreshToken();
            const { token, refreshToken } = response;
            authService.saveTokens(token, refreshToken);
        }catch{
            authService.logout();

        }
    }
}
export default ApiService;