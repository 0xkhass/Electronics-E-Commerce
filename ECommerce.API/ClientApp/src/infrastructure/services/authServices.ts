import type { User } from "../../shared/types/User";
import { api } from "../../core/api/axiosClient";

export const authService = {
  loginApi: async (email: string, password: string): Promise<User> => {
    const { data } = await api.post<User>("/api/auth/login", {
      email,
      password,
    });

    return data;
  },
  registerApi: async (
    email: string,
    password: string,
    userName: string,
    firstName: string,
    lastName: string,
  ): Promise<User> => {
    const { data } = await api.post<User>("/api/auth/register", {
      email,
      password,
      userName,
      firstName,
      lastName,
    });

    return data;
  },
  getCurrentUser: async (): Promise<User> => {
    const { data } = await api.get<User>("/api/auth/me");
    return data;
  },
  logoutApi: async (): Promise<void> => {
    await api.post("/api/auth/logout");
  },
};
