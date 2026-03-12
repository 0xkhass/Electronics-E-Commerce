import type { LoginCredentials } from './../../../shared/types/LoginCredentials';
import { useState } from "react";
import type { UseLoginReturn } from "../../../shared/types/UseLoginReturn";
import type { LoginError } from "../../../shared/types/LoginError";
import { login } from '../../../infrastructure/services/authServices';

export function useLogin(): UseLoginReturn{
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<LoginError | null>(null);

    const clearError = () => setError(null);

    const handleLogin = async ({ email, password } : LoginCredentials) => {
        setIsLoading(true);
        setError(null);

        try {
            const response = await login(email, password);

            const data = response.data;
            if (response.status !== 200) {
                // Handle common HTTP error codes
                if (response.status === 401) {
                    setError({field: 'general', message: 'Invalid email or password.'});
                } else if (response.status === 422) {
                    setError({field: 'general', 'message': data?.message ?? 'Validation failed.'});
                } else {
                    setError({field: 'general', message: data?.message ?? 'Something went wrong. Please try again.'});
                }
                return;
            }

            const { token, refreshToken } = data;

            console.log("Token: ", token);
            console.log("RefreshToken: ", refreshToken);

            window.location.href = '/';
        } catch {
            setError({ field: 'general', message: 'Network error. Please check your connection.'});
        } finally {
            setIsLoading(false);
        }
    };

    return { handleLogin, isLoading, error, clearError};

};