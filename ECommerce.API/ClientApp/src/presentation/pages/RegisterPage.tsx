import { useEffect, useState } from "react";
import { useRegister } from "../../app/hooks/useRegister";
import { getPasswordStrength } from "../../shared/utils/passwordStrengthChecker";
import { ChipIcon } from "../../assets/icons/ChipIcon";
import { Link } from "react-router-dom";
import { AtIcon } from "../../assets/icons/AtIcon";
import { CheckIcon } from "../../assets/icons/CheckIcon";
import { EmailIcon } from "../../assets/icons/EmailIcon";
import { LockIcon } from "../../assets/icons/LockIcon";
import { UserIcon } from "../../assets/icons/UserIcon";

import { Button } from "../components/shared/Button";
import { Input } from "../components/shared/Input";
import { PERKS } from "../../core/constants/perks";




export default function RegisterPage() {
    const [email, setEmail] = useState<string>('');
    const [userName, setUsername] = useState<string>('');
    const [fullName, setFullname] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [confirmPassword, setConfirmPassword] = useState<string>('');

    const [visible, setVisible] = useState<boolean>(false);

    const { handleRegister, isLoading, error, clearError} = useRegister();
    const strength = getPasswordStrength(password);

    useEffect(() => {
        const t = setTimeout(() => setVisible(true), 50);
        return () => clearTimeout(t);
    }, []);

    const handleSubmit = async (e: React.SubmitEvent) => {
        e.preventDefault();
        clearError();

        const nameParts = fullName.trim().split(/\s+/);
        const firstName = nameParts[0];
        const lastName = nameParts.slice(1).join(' ') || '';

        await handleRegister?.({email, userName, password, firstName, lastName})
    };

    return (
    <div className="min-h-screen w-full flex overflow-hidden bg-(--charcoal-900)">
 
      {/* ══════════════════════════════════
          LEFT — Brand Panel
      ══════════════════════════════════ */}
      <div className="hidden lg:flex lg:w-[52%] relative flex-col justify-between p-12 overflow-hidden">
 
        {/* Background layers */}
        <div className="absolute inset-0 bg-linear-to-br from-(--charcoal-900) via-[#1f3039] to-[#0f1e24]" />
 
        {/* Grid pattern */}
        <div
          className="absolute inset-0 opacity-[0.06]"
          style={{
            backgroundImage: `
              linear-gradient(var(--blue-gray-400) 1px, transparent 1px),
              linear-gradient(90deg, var(--blue-gray-400) 1px, transparent 1px)
            `,
            backgroundSize: '48px 48px',
          }}
        />
 
        {/* Glow orbs */}
        <div className="absolute top-[25%] left-[15%] w-100 h-100 rounded-full bg-(--tan-500) opacity-[0.07] blur-[100px] pointer-events-none" />
        <div className="absolute bottom-[15%] right-[10%] w-75 h-75 rounded-full bg-(--blue-gray-500) opacity-[0.08] blur-[80px] pointer-events-none" />
 
        {/* Logo */}
        <div className="relative z-10">
          <div className="flex items-center gap-3">
            <div className="p-2 rounded-xl bg-(--tan-500) text-(--charcoal-900)">
              <ChipIcon />
            </div>
            <span className="text-xl font-bold tracking-tight text-(--text-primary)">
              NEXUS<span className="text-(--tan-500)">TECH</span>
            </span>
          </div>
        </div>
 
        {/* Center content */}
        <div className="relative z-10 flex flex-col gap-8">
          <div>
            <p className="text-xs font-semibold tracking-[0.2em] text-(--blue-gray-400) uppercase mb-4">
              Join the Community
            </p>
            <h1 className="text-5xl font-bold text-(--text-primary) leading-[1.1] tracking-tight">
              Your Setup,
              <br />
              <span className="text-(--tan-400)">Elevated.</span>
            </h1>
            <p className="mt-4 text-(--khaki-500) text-base leading-relaxed max-w-sm">
              Create your account and unlock the full NEXUSTECH experience.
            </p>
          </div>
 
          {/* Perks list */}
          <div className="flex flex-col gap-3">
            {PERKS.map((perk) => (
              <div key={perk} className="flex items-center gap-3">
                <div className="w-6 h-6 rounded-full bg-[rgba(172,142,105,0.15)] border border-(--tan-500) flex items-center justify-center shrink-0 text-(--tan-500)">
                  <CheckIcon />
                </div>
                <span className="text-sm text-(--khaki-400)">{perk}</span>
              </div>
            ))}
          </div>
        </div>
 
        {/* Bottom */}
        <div className="relative z-10">
          <p className="text-xs text-(--text-muted)">
            Already have an account?{' '}
            <Link to="/login" className="text-(--tan-400) font-medium hover:text-(--tan-300) transition-colors">
              Sign in here
            </Link>
          </p>
        </div>
      </div>
 
      {/* ══════════════════════════════════
          RIGHT — Register Form
      ══════════════════════════════════ */}
      <div className="flex-1 flex items-center justify-center p-6 sm:p-10 bg-(--charcoal-800) relative">
 
        <div className="absolute top-0 right-0 w-64 h-64 bg-(--tan-500) opacity-[0.04] blur-[80px] rounded-full pointer-events-none" />
 
        <div
          className="w-full max-w-105 transition-all duration-700"
          style={{
            opacity: visible ? 1 : 0,
            transform: visible ? 'translateY(0)' : 'translateY(16px)',
          }}
        >
          {/* Mobile logo */}
          <div className="flex lg:hidden items-center gap-2 mb-8">
            <div className="p-1.5 rounded-lg bg-(--tan-500) text-(--charcoal-900)">
              <ChipIcon />
            </div>
            <span className="text-lg font-bold text-(--text-primary)">
              NEXUS<span className="text-(--tan-500)">TECH</span>
            </span>
          </div>
 
          {/* Heading */}
          <div className="mb-7">
            <h2 className="text-3xl font-bold text-(--text-primary) tracking-tight">
              Create account
            </h2>
            <p className="text-(--text-muted) text-sm mt-2">
              Fill in your details to get started
            </p>
          </div>
 
          {/* Global error */}
          {error?.field === 'general' && (
            <div className="mb-5 flex items-center gap-2.5 px-4 py-3 rounded-xl text-sm bg-[rgba(224,92,92,0.1)] border border-[rgba(224,92,92,0.25)] text-(--color-error)">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor" className="shrink-0">
                <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/>
              </svg>
              {error.message}
            </div>
          )}
 
          {/* Form */}
          <form onSubmit={handleSubmit} className="flex flex-col gap-4">
 
            {/* Full Name + Username side by side */}
            <div className="grid grid-cols-2 gap-3">
              <Input
                label="Full Name"
                type="text"
                id="fullName"
                placeholder="John Doe"
                value={fullName}
                onChange={(e: React.SubmitEvent) => setFullname(e.target.value)}
                icon={<UserIcon />}
                required
                autoComplete="name"
              />
              <Input
                label="Username"
                type="text"
                id="username"
                placeholder="johndoe"
                value={userName}
                onChange={(e: React.SubmitEvent) => setUsername(e.target.value)}
                icon={<AtIcon />}
                required
                autoComplete="username"
              />
            </div>
 
            <Input
              label="Email address"
              type="email"
              id="email"
              placeholder="you@example.com"
              value={email}
              onChange={(e: React.SubmitEvent) => setEmail(e.target.value)}
              icon={<EmailIcon />}
              error={error?.field === 'email' ? error.message : undefined}
              required
              autoComplete="email"
            />
 
            <div className="flex flex-col gap-1.5">
              <Input
                label="Password"
                type="password"
                id="password"
                placeholder="Min. 8 characters"
                value={password}
                onChange={(e: React.SubmitEvent) => setPassword(e.target.value)}
                icon={<LockIcon />}
                error={error?.field === 'password' ? error.message : undefined}
                required
                autoComplete="new-password"
              />
              {/* Password strength bar */}
              {password && (
                <div className="flex items-center gap-2 px-1">
                  
                  <span
                    className="text-xs font-medium transition-colors duration-300"
                    style={{ color: strength.color }}
                  >
                    {strength.label}
                  </span>

                  <div className="flex gap-1 flex-1">
                    {[1, 2, 3, 4].map((i) => (
                      <div
                        key={i}
                        className="h-1 flex-1 rounded-full transition-all duration-300"
                        style={{
                          backgroundColor:
                            i <= strength.score ? strength.color : 'var(--charcoal-500)',
                        }}
                      />
                    ))}
                  </div>
                </div>
              )}
            </div>
 
            <Input
              label="Confirm Password"
              type="password"
              id="confirmPassword"
              placeholder="Repeat your password"
              value={confirmPassword}
              onChange={(e: React.SubmitEvent) => setConfirmPassword(e.target.value)}
              icon={<LockIcon />}
              error={error?.field === 'confirmPassword' ? error.message : undefined}
              required
              autoComplete="new-password"
            />
 
            <Button
              type="submit"
              variant="primary"
              fullWidth
              isLoading={isLoading}
              className="mt-1 py-3.5 text-base"
            >
              Create Account
            </Button>
          </form>
 
          {/* Divider */}
          <div className="flex items-center gap-3 my-5">
            <div className="flex-1 h-px bg-(--border-subtle)" />
            <span className="text-xs text-(--text-muted)">or</span>
            <div className="flex-1 h-px bg-(--border-subtle)" />
          </div>
 
          {/* Sign in link */}
          <p className="text-center text-sm text-(--text-muted)">
            Already have an account?{' '}
            <Link
              to="/login"
              className="text-(--tan-400) font-semibold hover:text-(--tan-300) transition-colors"
            >
              Sign in
            </Link>
          </p>
 
          {/* Footer */}
          <p className="text-center text-xs text-(--charcoal-500) mt-7">
            By creating an account you agree to our{' '}
            <Link to="/terms" className="hover:text-(--text-muted) transition-colors underline underline-offset-2">
              Terms
            </Link>{' '}
            &{' '}
            <Link to="/privacy" className="hover:text-(--text-muted) transition-colors underline underline-offset-2">
              Privacy Policy
            </Link>
          </p>
        </div>
      </div>
    </div>
  )

};