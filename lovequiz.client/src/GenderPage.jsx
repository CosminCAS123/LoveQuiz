import React from 'react';
import './App.css';
import './GenderBlock.scss';
import Header from './Header.jsx'
import Footer from './Footer.jsx'
import './tailwind.css';
import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { useRef } from 'react';
import FloatingHearts from './FloatingHeats.jsx';

function GenderBlock() {
    const navigate = useNavigate();

    return (
      <>
      <FloatingHearts />
        <div className="header-wrapper wrapper">
          <Header />
        </div>

        <div className="gender-block-wrapper wrapper ccontainer">
      
            <div className="animate-slide-down main-block__title z-10 text-center">
                <div className="main-block__title">
                    <em>TU ESTI DE VINA!</em>
                    </div>
                    <div className="main-block__title--subtitle pl-3">
                  <em>sau poate sunt eu...?</em>
              </div>
            </div>

            <div className="gender-block flex">
                <div className="gender-block__title animate-fade-in z-10">
                    alege-ti genul pentru a afla:
                    <span>
                      *in doar 5 minute*
                    </span>
                </div>

                <div className="gender-block__genders animate-grow">
                    <button className="gender-block__genders-box z-21" onClick={() => navigate('/gender/male')}>
                        <img src="/assets/man-3.png" className="gender-block__genders--male-image"/>

                        <span className="gender-block__genders--male-text">Barbat</span>
                    </button>
                    <button className="gender-block__genders-box" onClick={() => navigate('/gender/female')}>
                        <img src="/assets/woman-2.png" className="gender-block__genders--female-image"/>

                        <span className="gender-block__genders--female-text">Femeie</span>
                    </button>
                </div>
            </div>
        </div>

        <div className="footer-wrapper">
          <Footer />
        </div> 
      </>
    );
}

export function QuizComponent({ gender }) {
  const quizRef  = useRef(null);
  const navigate = useNavigate();
  const [animationState, setAnimationState] = useState("grow");

  const [questions, setQuestions] = useState([]);
  const [error,     setError]     = useState(null);
  const [current,   setCurrent]   = useState(0);
  const [selectedAnswers, setSelectedAnswers] = useState({}); 

  useEffect(() => {
      fetch(`/api/quiz/questions?gender=${encodeURIComponent(gender)}`)//this is for production, for development use       fetch(`https://localhost:7279/api/quiz/questions?gender=${encodeURIComponent(gender)}`)

      .then(r => {
        if (!r.ok) throw new Error("HTTP " + r.status);
        return r.json();
      })
      .then(data => {
        setQuestions(data);
        setCurrent(0);
      })
      .catch(err => setError(err.message));
  }, [gender]);

  if (error)            return <p className="error">Nu se poate incarca intrebarea.</p>;
  if (questions.length === 0) return <p>Se incarca…</p>;

  const q      = questions[current];
  const isLast = current === questions.length - 1;

  const handleRadioChange = (idx) =>
    setSelectedAnswers(prev => ({ ...prev, [q.id]: idx }));

  const handleNext = () => {
    if (selectedAnswers[q.id] === undefined) {
      alert("Te rugăm să alegi un răspuns înainte de a continua.");
      return;                                
    }
    if (isLast) {
      navigate("/results", { state: { answers: selectedAnswers, questions } });
    } else {
      setCurrent(p => p + 1);
    }
  };

  const handlePrev = () => setCurrent(p => Math.max(p - 1, 0));

  return (
    <div
      className={`quiz-wrapper pl-12 relative questions-wrapper mt-auto mb-auto flex max-h-[70vh] ${
        animationState === "fade" ? "animate-fade-out-left" :
        animationState === "grow" ? "animate-grow" : ""
      }`}
      ref={quizRef}
    >
      <div className="w-full">

      <div className="w-full max-w-xl mx-auto pb-4">
        <div className="h-2 w-full bg-rose-100 rounded-full overflow-hidden">
          <div
            className="h-full bg-color-magenta transition-all duration-500"
            style={{ width: `${((current + 1) / questions.length) * 100}%` }}
          />
        </div>
        <p className="text-xs text-center pt-1 text-color-primary">
          Intrebarea {current + 1} din {questions.length}
        </p>
      </div>


        <div className="question font-extrabold text-xl pb-6 text-center ">{q.question}</div>

        <ul className="answers space-y-4 answers-wrapper ">
          {q.answers.map((ans, i) => (
            <li key={i} className="answer-row">

              <input
                type="radio"
                id={`q${q.id}-${i}`}
                name={`q${q.id}`}
                value={i}
                checked={selectedAnswers[q.id] === i}
                onChange={() => {
                  setSelectedAnswers(prev => ({ ...prev, [q.id]: i }));
                  setTimeout(() => {
                    if (isLast) {
                      navigate("/results", { state: { answers: { ...selectedAnswers, [q.id]: i }, questions } });
                    } else {
                      setCurrent(p => p + 1);
                    }
                  }, 250); 
                }}
                className="answer-radio"
              />

              <label
                htmlFor={`q${q.id}-${i}`}
                className="
                  flex cursor-pointer select-none items-center gap-3 rounded-lg
                  px-4 py-3 text-md custom-answer-choices answer-label  /* transition removed */
                "
                onClick={() => {
                  if (animationState === "fade") return;
                
                  setSelectedAnswers(prev => ({ ...prev, [q.id]: i }));
                  setAnimationState("fade");
                
                  setTimeout(() => {
                    if (isLast) {
                      navigate("/results", { state: { answers: { ...selectedAnswers, [q.id]: i }, questions } });
                    } else {
                      setCurrent(p => p + 1);
                      setAnimationState("grow");
                    }
                  }, 400); 
                }}
              >
                {ans.answer}
              </label>
            </li>
          ))}
        </ul>

        <div className="pt-10 flex gap-4">
          {current > 0 && (
            <button onClick={handlePrev} className="quiz-button custom-quiz-button">
              Inapoi
            </button>
          )}

          {/* Button to go to the next question - i do not think we need it anymore */}
          {/* <button onClick={handleNext} className="ml-auto quiz-button">
            {isLast ? "Afla raspunsul" : "Inainte"}
          </button> */}
        </div>
      </div>
    </div>
  );
}

export function FemaleBlock() {



  return(

    <>
    <FloatingHearts />
    <div className="header-wrapper wrapper">
      <Header />
    </div>

    <div className = "flex bg-color-primary questions-page-height">
      
      <QuizComponent gender="female"/>

    </div>

    <div className="footer-wrapper">
      <Footer />
    </div> 

    </>
                
  );
}

export function MaleBlock() {


  return(

    <>
    <FloatingHearts />
    <div className="header-wrapper wrapper">
      <Header />
    </div>

    <div className = "flex bg-color-primary questions-page-height">
      
      <QuizComponent gender="male" />

    </div>

    <div className="footer-wrapper">
      <Footer />
    </div> 

    </>
                
  );
}

export default GenderBlock;
