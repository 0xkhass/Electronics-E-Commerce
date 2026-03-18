import type { PasswordStrengthChecker } from "../types/ui/PasswordStrengthCheckerType";

// password strength checker
export function getPasswordStrength(password: string): PasswordStrengthChecker {
    if (!password) return { score: 0, label: '', color: ''}

    let score = 0;
    if (password.length >= 8) score++
    if(/[A-Z]/.test(password)) score++
    if(/[0-9]/.test(password)) score++
    if(/[^A-Za-z0-9]/.test(password)) score++

    if (score <= 1) return { score, label:'Weak', color: 'var(--color-error)'}
    if (score === 2) return { score, label: 'Fair', color: 'var(--color-warning)'}
    if (score === 3) return { score, label: 'Good', color: 'var(--color-info)'}

    return { score, label: 'Strong', color:'var(--color-success)'}
};