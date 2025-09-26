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
import { RotatingLines } from 'react-loader-spinner';

function GenderBlock() {
    const navigate = useNavigate();

    return (
      <>
      <FloatingHearts />
        <div className="header-wrapper">
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
                    <button 
                    className="gender-block__genders-box z-21" 
                    onClick={() => {
                      localStorage.setItem('gender', 'male');
                      navigate('/gender/male');
                    }}>
                        <img src="/assets/man-3.png" className="gender-block__genders--male-image"/>

                        <span className="gender-block__genders--male-text">Barbat</span>
                    </button>

                    <button 
                    className="gender-block__genders-box" 
                    onClick={() => {
                      localStorage.setItem('gender', 'female');
                      navigate('/gender/female');
                    }}>
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

  const [showEmailModal, setShowEmailModal] = useState(false);
  const [email, setEmail] = useState("");

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

  if (error)            return <p className="error text-color-primary m-auto">Nu se poate incarca intrebarea.</p>;
  if (questions.length === 0) return <div className='text-color-primary m-auto'> <RotatingLines color="#A86A7A" width="40" />;</div>

  const q      = questions[current];
  const isLast = current === questions.length - 1;

  const handlePrev = () => setCurrent(p => Math.max(p - 1, 0));

  return (
    <>
      <div
        className={`quiz-wrapper pl-12 relative questions-wrapper mt-auto mb-auto flex max-h-[70vh] 
          ${animationState === "fade" ? "animate-fade-out-left" : ""}
          ${animationState === "grow" ? "animate-grow" : ""}
          ${showEmailModal ? "pointer-events-none" : ""}
        `}
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
                        setShowEmailModal(true);
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
                        setShowEmailModal(true);
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


    {showEmailModal && (
        <div className="fixed inset-0 bg-color-primary bg-opacity-50 h-[98vh] flex pointer-events-auto items-center justify-center z-50 animate-grow">
          <div className="bg-white p-6 rounded-lg shadow-lg w-full max-w-md mx-4">
            <h2 className="text-lg font-bold mb-4 text-center text-color-primary">Introdu adresa ta de email pentru a iti salva rezultatele</h2>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="w-full border px-3 py-2 rounded mb-4"
              placeholder="exemplu@email.com"
            />
            <div className="flex justify-between">
              <button
                className="quiz-button bg-gray-300 text-black"
                onClick={() => navigate("/")}
              >
                Anulează
              </button>
                          <button
                              className="quiz-button"
                              onClick={async () => {
                                  if (!email.includes("@")) {
                                      alert("Te rugăm să introduci o adresă validă.");
                                      return;
                                  }

                                  try {
                                      const gender = localStorage.getItem("gender"); // already saved earlier

                                      // 1) log the session
                                      const res = await fetch("/api/quiz/log-free-session", {
                                          method: "POST",
                                          headers: { "Content-Type": "application/json" },
                                          body: JSON.stringify({
                                              email,
                                              gender
                                          })
                                      });
                                      if (!res.ok) throw new Error(`log-free-session HTTP ${res.status}`);
                                      const { sessionId } = await res.json();

                                      // 2) save it for full-report later
                                      localStorage.setItem("quiz.sessionId", sessionId);

                                      // 3) go to results page
                                      navigate("/results", {
                                          state: { answers: selectedAnswers, questions, email }
                                      });
                                  } catch (err) {
                                      console.error(err);
                                      alert("Nu am putut crea sesiunea. Încearcă din nou.");
                                  }
                              }}
                          >
                              Continuă
                          </button>
            </div>
          </div>
        </div>
  )}
  </>
);}

export function FemaleBlock() {



  return(

    <>
    <FloatingHearts />
    <div className="header-wrapper ">
      <Header />
    </div>

    <div className = "flex bg-color-primary questions-page-height">
      
      <QuizComponent gender="female"/>

    </div>

    <div className="footer-wrapper h-[8vh]">
      <Footer />
    </div> 

    </>
                
  );
}

export function MaleBlock() {


  return(

    <>
    <FloatingHearts />
    <div className="header-wrapper">
      <Header />
    </div>

    <div className = "flex bg-color-primary questions-page-height">
      
      <QuizComponent gender="male" />

    </div>

    <div className="footer-wrapper h-[8vh]">
      <Footer />
    </div> 

    </>
                
  );
}

export default GenderBlock;
