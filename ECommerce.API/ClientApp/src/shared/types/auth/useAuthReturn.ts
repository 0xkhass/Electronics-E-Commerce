import type { AuthError } from "./AuthError";
import type { LoginCredentials } from "./LoginCredentials";
import type { RegisterCredentials } from "./RegisterCredentials";

export interface UseAuthReturn {
    handleRegister?: (credentail: RegisterCredentials) => Promise<void>;
    handleLogin?: (credential: LoginCredentials) => Promise<void>;
    isLoading: boolean
    error: AuthError | null;
    clearError: () => void;
};