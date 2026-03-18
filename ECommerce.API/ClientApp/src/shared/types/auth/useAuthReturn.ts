import type { AuthError } from "./AuthError";
import type { LoginCredentials } from "./AuthCredentials";
import type { RegisterCredentials } from "./AuthCredentials";
export interface UseAuthReturn {
    handleRegister?: (credentail: RegisterCredentials) => Promise<void>;
    handleLogin?: (credential: LoginCredentials) => Promise<void>;
    isLoading: boolean
    error: AuthError | null;
    clearError: () => void;
};