import { useState } from "react";
import type { InputProps } from "../../shared/types/components/InputProps";

export const Input: React.FC<InputProps> = ({
  label,
  error,
  icon,
  type = "text",
  className = "",
  id,
  ...props
}) => {
  const [showPassword, setShowPassword] = useState(false);
  const isPassword = type === "password";
  const inputType = isPassword ? (showPassword ? "text" : "password") : type;
  const inputId = id || label?.toLowerCase().replace(/\s+/g, "-");

  return (
    <div className="flex flex-col gap-1.5 w-full">
      {label && (
        <label
          htmlFor={inputId}
          className="text-sm font-medium text-(--khaki-400) tracking-wide"
        >
          {label}
        </label>
      )}

      <div className="relative group">
        {/* Left Icon */}
        {icon && (
          <span
            className="absolute left-3 top-1/3 -transalte-y-1/2 text-(--text-muted) 
                    group-focus-within:text-(--tan-500) transition-colors duration-200"
          >
            {icon}
          </span>
        )}

        <input
          id={inputId}
          type={inputType}
          className={`
            w-full bg-(--bg-input) text-(--text-primary)
            border border-(--border-default) rounded-[10px]
            py-3 pr-8 text-sm
            ${icon ? "pl-10" : "pl-4"}
            placeholder:text-(--text-muted)
            outline-none transition-all duration-200
            focus:border-(--tan-500) focus:shadow-[0_0_0_3px_rgba(172,142,105,0.15)]
            ${error ? "border-(--color-error) focus:border-(--color-error) focus:shadow-[0_0_0_3px_rgba(224,92,92,0.15)]" : ""}
            ${isPassword ? "pr-11" : ""}
            ${className}
            `}
            {...props}
        />

        {/* Password toggle */}
        {isPassword && (
            <button
            type="button"
            onClick={() => setShowPassword((v) => !v)}
            className="absolute right-3.5 top-1/2 -translate-y-1/2 text-(--text-muted) hover:text-(--khaki-400) transition-colors duration-200 focus:outline-none"
            tabIndex={-1}
            >   {showPassword ? (
              // Eye-off icon
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                <path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94"/>
                <path d="M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19"/>
                <line x1="1" y1="1" x2="23" y2="23"/>
              </svg>
            ) : (
              // Eye icon
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/>
                <circle cx="12" cy="12" r="3"/>
              </svg>
            )}
          </button>
        )}
      </div>

      {/* Error message */}
      {error && (
        <p className="text-xs text-(--color-error) flex items-center gap-1 mt-0.5">
          <svg width="12" height="12" viewBox="0 0 24 24" fill="currentColor">
            <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/>
          </svg>
          {error}
        </p>
      )}
    </div>
  )
};

