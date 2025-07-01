import React from 'react';
import './App.css';
import './GenderBlock.scss';
import Header from './Header.jsx'
import Footer from './Footer.jsx'
import './tailwind.css';
import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { useInView } from 'react-intersection-observer';
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
            <div className="gender-block flex">
                <div className="gender-block__title animate-fade-in z-10">
                    Alege-ti genul:
                </div>

                <div className="gender-block__genders animate-grow z-10">
                    <button className="gender-block__genders-box">
                        <img src="../assets/man-3.png" className="gender-block__genders--male-image"/>

                        <span className="gender-block__genders--male-text">Barbat</span>
                    </button>
                    <button className="gender-block__genders-box" onClick={() => navigate('female')}>
                        <img src="../assets/woman-2.png" className="gender-block__genders--female-image"/>

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

export function FemaleBlock() {

  const quizRef = useRef(null);  
  const [questions, setQuestions] = useState([]);   
  const [error,     setError]     = useState(null);

  useEffect(() => {
    fetch('http://localhost:5023/api/quiz/questions?gender=female')
      .then(r => {
        if (!r.ok) throw new Error('HTTP ' + r.status);
        return r.json();
      })
      .then(setQuestions)          
      .catch(setError);
  }, []);

  const [ref1, inView1] = useInView({ triggerOnce: true, threshold: 0.3 });
  const [ref2, inView2] = useInView({ triggerOnce: true, threshold: 0.3 });

  return(

    <>
    <FloatingHearts />
    <div className="header-wrapper wrapper">
      <Header />
    </div>

    <div class = "flex bg-color-primary">
      
      <div className="quiz-wrapper gender-block-wrapper pl-12 relative" ref={quizRef}>
        {error && <p className="error">Nu se poate încărca întrebarea.</p>}

        {!error && questions.length === 0 && <p>Se încarcă…</p>}

        {questions.map((q, idx) => (
          <div key={idx} className="mb-20 z-10 w-fit">
            <div className="question font-extrabold text-2xl">{q.question}</div>
            <ul className="answers space-y-4">
              {q.answers.map((ans, i) => (
                <li className="mb-2" key={i}>
                  <label className="text-color-primary flex items-start gap-3 cursor-pointer text-lg">
                    <input type="radio" name={`q${q.id}`} value={i} className="text-color-primary mt-1 h-5 w-5" />
                    {ans.answer}
                  </label>
                </li>
              ))}
            </ul>
          </div>
        ))}
      </div> 

      {/* <div className="fixed bottom-0 right-0">
        <div className="animate-up-left-scroll-root bg-red-600">.</div>
      </div> */}
    </div>

    <div className="footer-wrapper">
      <Footer />
    </div> 

    </>
                
  );
}

export default GenderBlock;
