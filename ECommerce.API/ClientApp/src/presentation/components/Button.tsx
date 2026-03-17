import type { ButtonProps } from "../../shared/types/components/ButtonProps"

export const Button: React.FC<ButtonProps> = ({
  variant = 'primary',
  isLoading = false,
  fullWidth = false,
  children,
  className = '',
  disabled,
  ...props
}) => {
  const base =
    'relative inline-flex items-center justify-center gap-2 font-semibold text-sm rounded-[10px] transition-all duration-200 active:scale-[0.98] focus:outline-none focus-visible:ring-2 focus-visible:ring-[var(--tan-500)] focus-visible:ring-offset-2 focus-visible:ring-offset-[var(--charcoal-800)] disabled:opacity-40 disabled:cursor-not-allowed'

  const variants = {
    primary:
      'bg-[var(--tan-500)] text-(--charcoal-900) px-6 py-3 hover:bg-(--tan-400) hover:shadow-[0_4px_20px_rgba(172,142,105,0.35)]',
    secondary:
      'bg-transparent text-(--blue-gray-300) border border-(--blue-gray-400) px-6 py-3 hover:bg-[rgba(92,132,145,0.12)] hover:border-(--blue-gray-300)',
    ghost:
      'bg-transparent text-(--text-muted) px-4 py-2 hover:text-(--text-primary) hover:bg-[rgba(255,255,255,0.05)]',
  }

  return (
    <button
      className={`${base} ${variants[variant]} ${fullWidth ? 'w-full' : ''} ${className}`}
      disabled={disabled || isLoading}
      {...props}
    >
      {isLoading ? (
        <>
          <span className="w-4 h-4 rounded-full border-2 border-current border-t-transparent animate-spin" />
          <span>Signing in...</span>
        </>
      ) : (
        children
      )}
    </button>
  )
}