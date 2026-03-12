import { api } from '../api/axiosClient';

export const login = (email: string, password: string) => {
    return api.post( 'api/Auth/login',{email, password});
};