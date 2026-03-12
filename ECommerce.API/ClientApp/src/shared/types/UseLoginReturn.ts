import type { LoginCredentials } from "./LoginCredentials";
import type { LoginError } from "./LoginError";

export interface UseLoginReturn {
    handleLogin: (credential: LoginCredentials) => Promise<void>;
    isLoading: boolean
    error: LoginError | null;
    clearError: () => void;
};