import { API } from './constant';
import axios from "axios";

const instance = axios.create({
    baseURL: API,
});

instance.defaults.withCredentials = true;

instance.interceptors.request.use(
    function (config : any) {
        var token = localStorage.getItem("accessToken");
        config.headers.Authorization = token ? `Bearer ${token}` : "";
        return config;
    },
    function (error) {
        return Promise.reject(error);
    }
);

export default instance;