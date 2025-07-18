import { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";

const testimonials = [
  {
    image: "/path/to/image1.jpg",
    stars: 5,
    name: "Andrei Popescu",
    content: "Site-ul mi-a deschis ochii. Am avut o discutie reala cu partenera mea dupa ani intregi.",
    date: "10 iulie 2025",
  },
  {
    image: "/path/to/image2.jpg",
    stars: 4,
    name: "Ioana Marinescu",
    content: "E impresionant cat de bine m-a descris raportul! Merita fiecare secunda.",
    date: "12 iulie 2025",
  },
  {
    image: "/path/to/image3.jpg",
    stars: 5,
    name: "Mihai Stan",
    content: "M-a facut sa constientizez lucruri despre mine pe care le ignoram complet.",
    date: "15 iulie 2025",
  },
];

const swipeConfidenceThreshold = 10000;


function TestimonialCard({ testimonial }) {
    return (
      <div className="text-center px-4 py-6 bg-color-primary rounded-xl shadow-lg">
        <img
          src={testimonial.image}
          alt={testimonial.name}
          className="w-16 h-16 rounded-full mx-auto mb-2"
        />
        <div className="text-yellow-400 mb-1">{"★".repeat(testimonial.stars)}</div>
        <h4 className="font-semibold text-color-primary">{testimonial.name}</h4>
        <p className="text-sm text-gray-500 italic mb-2">— {testimonial.date}</p>
        <p className="text-sm text-gray-700 max-w-sm mx-auto">
          {testimonial.content}
        </p>
      </div>
    );
  }

export default function TestimonialSlider() {
  const [[page, direction], setPage] = useState([0, 0]);

  const paginate = (newDirection) => {
    setPage([
      (page + newDirection + testimonials.length) % testimonials.length,
      newDirection,
    ]);
  };

  return (
    <div className="relative max-w-md  mx-auto px-8 min-h-[250px] -mt-16">
      <AnimatePresence initial={false} custom={direction}>
        <motion.div
          key={page}
          className="absolute w-full -top-1/4 left-0 min-h-[200px]"
          custom={direction}
          initial={{ opacity: 0, x: direction > 0 ? 100 : -100 }}
          animate={{ opacity: 1, x: 0 }}
          exit={{ opacity: 0, x: direction < 0 ? 100 : -100 }}
          transition={{ duration: 0.4 }}
          drag="x"
          dragConstraints={{ left: 0, right: 0 }}
          dragElastic={1}
          onDragEnd={(e, { offset, velocity }) => {
            const swipe = Math.abs(offset.x) * velocity.x;

            if (swipe < -swipeConfidenceThreshold) {
              paginate(1);
            } else if (swipe > swipeConfidenceThreshold) {
              paginate(-1);
            }
          }}
        >
          <TestimonialCard testimonial={testimonials[page]} />
        </motion.div>
      </AnimatePresence>

      {/* Arrows */}
      <div className="custom-button">
        <button
            onClick={() => paginate(-1)}
            className="absolute left-4 top-1/8 -translate-y-1/2 p-2 opacity-30 hover:opacity-60 transition"
        >
            <FaChevronLeft className="text-[#874B5C]" />
        </button>

        <button
            onClick={() => paginate(1)}
            className="absolute right-4 top-1/8 -translate-y-1/2 p-2 opacity-30 hover:opacity-60 transition"
        >
            <FaChevronRight className="text-[#874B5C]" />
        </button>
      </div>

      {/* Dots */}
      <div className="flex justify-center space-x-2 mt-40 relative z-10">
        {testimonials.map((_, i) => (
          <span
            key={i}
            className={`w-2 h-2 rounded-full transition ${
              i === page ? "bg-color-primary" : "bg-gray-300"
            }`}
          />
        ))}
      </div>
    </div>
  );
}