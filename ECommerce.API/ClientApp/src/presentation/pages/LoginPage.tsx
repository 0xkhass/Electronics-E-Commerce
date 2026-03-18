import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { useLogin } from '../../app/hooks/useLogin'
import { ChipIcon } from '../../assets/icons/ChipIcon'
import { SPECS } from '../../shared/types/components/SpecsProps'
import { EmailIcon } from '../../assets/icons/EmailIcon'
import { LockIcon } from '../../assets/icons/LockIcon'
import { Button } from '../components/shared/Button'
import { Input } from '../components/shared/Input'

export default function LoginPage() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [visible, setVisible] = useState(false)

  const { handleLogin, isLoading, error, clearError } = useLogin()

  // Staggered mount animation
  useEffect(() => {
    const t = setTimeout(() => setVisible(true), 50)
    return () => clearTimeout(t)
  }, [])

  const handleSubmit = async (e: React.SubmitEvent) => {
    e.preventDefault()
    clearError()
    await handleLogin?.({ email, password})
  }

  return (
    <div className="min-h-screen w-full flex overflow-hidden bg-(--charcoal-900)">
      {/* ══════════════════════════════════
          LEFT — Brand Panel
      ══════════════════════════════════ */}
      <div className="hidden lg:flex lg:w-[52%] relative flex-col justify-between p-4 overflow-hidden">

        {/* Background layers */}
        <div className="absolute inset-0 bg-linear-to-br from-(--charcoal-900) via-[#1f3039] to-[#0f1e24]" />

        {/* Grid pattern overlay */}
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

        {/* Glow orb */}
        <div className="absolute top-[30%] left-[20%] w-105 h-105 rounded-full bg-(--blue-gray-500) opacity-[0.08] blur-[100px] pointer-events-none" />
        <div className="absolute bottom-[10%] right-[5%] w-70 h-70 rounded-full bg-(--tan-500) opacity-[0.07] blur-[80px] pointer-events-none" />

        {/* Content */}
        <div className="relative z-10">
          {/* Logo */}
          <div className="flex items-center gap-4 mb-10">
            <div className="p-2 rounded-xl bg-(--tan-500) text-(--charcoal-900)">
              <ChipIcon />
            </div>
            <span className="text-xl font-bold tracking-tight text-(--text-primary)">
              NEXUS<span className="text-(--tan-500)">TECH</span>
            </span>
          </div>
        </div>

        {/* Center headline */}
        <div className="relative z-10 flex flex-col gap-6">
          <div>
            <p className="text-xs font-semibold tracking-[0.2em] text-(--blue-gray-400) uppercase mb-4">
              Premium Electronics
            </p>
            <h1 className="text-5xl font-bold text-(--text-primary) leading-[1.1] tracking-tight">
              Power Meets
              <br />
              <span className="text-(--tan-400)">Precision.</span>
            </h1>
            <p className="mt-4 text-(--khaki-500) text-base leading-relaxed max-w-sm">
              Laptops, desktops, and components curated for professionals and enthusiasts alike.
            </p>
          </div>

          {/* Spec cards */}
          <div className="grid grid-cols-2 gap-3 mt-2">
            {SPECS.map((spec) => (
              <div
                key={spec.label}
                className="bg-[rgba(255,255,255,0.04)] border border-(--border-subtle) rounded-xl p-4 backdrop-blur-sm"
              >
                <p className="text-[10px] font-bold tracking-[0.15em] text-(--blue-gray-400) uppercase mb-1">
                  {spec.label}
                </p>
                <p className="text-sm font-semibold text-(--khaki-300)">{spec.value}</p>
              </div>
            ))}
          </div>
        </div>

        {/* Bottom tagline */}
        <div className="relative z-10">
          <p className="text-xs text-(--text-muted)">
            Trusted by <span className="text-(--khaki-400) font-medium">12,000+</span> customers worldwide
          </p>
        </div>
      </div>

      {/* ══════════════════════════════════
          RIGHT — Login Form
      ══════════════════════════════════ */}
      <div className="flex-1 flex items-center justify-center p-6 sm:p-10 bg-(--charcoal-800) relative">

        {/* Subtle top-right glow */}
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
          <div className="mb-8">
            <h2 className="text-3xl font-bold text-(--text-primary) tracking-tight">
              Welcome back
            </h2>
            <p className="text-(--text-muted) text-sm mt-2">
              Sign in to your account to continue
            </p>
          </div>

          {/* Global error */}
          {error?.field === 'general' && (
            <div
              className="mb-5 flex items-center gap-2.5 px-4 py-3 rounded-xl text-sm
                bg-[rgba(224,92,92,0.1)] border border-[rgba(224,92,92,0.25)] text-(--color-error)"
            >
              <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor" className="shrink-0">
                <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/>
              </svg>
              {error.message}
            </div>
          )}

          {/* Form */}
          <form onSubmit={handleSubmit} className="flex flex-col gap-5">
            <Input
              label="Email address"
              type="email"
              id="email"
              placeholder="you@example.com"
              value={email}
              onChange={(e: React.SubmitEvent) => setEmail(e.target.value)}
              icon={<EmailIcon />}
              required
              autoComplete="email"
            />

            <Input
              label="Password"
              type="password"
              id="password"
              placeholder="Enter your password"
              value={password}
              onChange={(e: React.SubmitEvent) => setPassword(e.target.value)}
              icon={<LockIcon />}
              required
              autoComplete="current-password"
            />
            <Button
              type="submit"
              variant="primary"
              fullWidth
              isLoading={isLoading}
              className="mt-1 py-3.5 text-base"
            >
              Sign in
            </Button>
          </form>

          {/* Divider */}
          <div className="flex items-center gap-3 my-6">
            <div className="flex-1 h-px bg-(--border-subtle)" />
            <span className="text-xs text-(--text-muted)">or</span>
            <div className="flex-1 h-px bg-(--border-subtle)" />
          </div>

          {/* Sign up link */}
          <p className="text-center text-sm text-(--text-muted)">
            Don't have an account?{' '}
            <Link
              to="/register"
              className="text-(--tan-400) font-semibold hover:text-(--tan-300) transition-colors"
            >
              Create one free
            </Link>
          </p>

          {/* Footer note */}
          <p className="text-center text-xs text-(--charcoal-500) mt-8">
            By signing in you agree to our{' '}
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
}