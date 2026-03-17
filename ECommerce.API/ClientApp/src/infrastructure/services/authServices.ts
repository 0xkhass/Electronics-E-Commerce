import type { User } from '../../shared/types/User';
import { api } from '../api/axiosClient';

export const loginApi = async (email: string, password: string) : Promise<User> => {
    const { data} = await api.post<User>('/api/Auth/login', {
        email,
        password
    });
    return data;
};

export const registerApi =  async (
    email: string,
    password: string,
    userName: string,
    firstName: string,
    lastName: string,
) : Promise<User> => {
    const { data } = await api.post<User>('/api/auth/register', {
        email,
        password,
        userName,
        firstName,
        lastName

    });

    console.log("AuthService => Register: ", data);
    return data
};


// authServices.ts
export const getCurrentUser = async (): Promise<User> => {
    const { data } = await api.get<User>('/api/auth/me');
    return data;
};

export const logoutUser = async (): Promise<void> => {
    await api.post('/api/auth/logout');
};