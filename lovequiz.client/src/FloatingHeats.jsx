import React, { useRef } from "react";
import { Heart } from "lucide-react";
import "./index.css";

// How many hearts? Crank it up or down.
const NUMBER_OF_HEARTS = 10;

/**
 * FloatingHearts (scroll-proof)
 * ---------------------------------------------------------------
 * A fixed, full-viewport layer that continuously floats hearts
 * upward.  Because the layer itself never translates with page
 * scroll, the animation feels independent of user scrolling.
 *
 * - `pointer-events-none` keeps it non-interactive.
 * - `fixed inset-0` ties it to the viewport so hearts don't budge
 *   when the document scrolls.
 */
export default function FloatingHearts() {
  /* Create a one-time randomised list of heart props */
  const hearts = useRef(
    Array.from({ length: NUMBER_OF_HEARTS }, (_, id) => ({
      id,
      left: Math.random() * 100,        // 0–100 % across viewport
      size: 18 + Math.random() * 34,    // 18–52 px
      delay: Math.random() * 5,         // 0–5 s stagger
      duration: 8 + Math.random() * 12, // 8–20 s per loop
      opacity: 0.15 + Math.random() * 0.25,
      hue: "text-rose-400"
    }))
  ).current;

  return (
    <div className="pointer-events-none fixed inset-0 z-20 overflow-hidden">
      {hearts.map((h) => (
        <Heart
          key={h.id}
          className={`absolute ${h.hue}`}
          style={{
            left: `${h.left}%`,
            bottom: `-${h.size}px`,
            width: h.size,
            height: h.size,
            opacity: h.opacity,
            animation: `floatUp ${h.duration}s linear ${h.delay}s infinite`,
          }}
          fill="currentColor"
        />
      ))}

      {/* keyframes inline so the component is  drop-in-ready */}
      <style>{`
        @keyframes floatUp {
          0%   { transform: translateY(0); }
          100% { transform: translateY(-120vh); }
        }
      `}</style>

    </div>
  );
}
