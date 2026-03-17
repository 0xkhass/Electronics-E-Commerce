export interface AuthError {
    field?: 'email' | 'password' | 'general' | 'confirmPassword';
    message: string;
};
