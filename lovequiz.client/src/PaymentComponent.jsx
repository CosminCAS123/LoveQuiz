import Header from "./Header";
import Footer from "./Footer";
import { useState, useEffect } from "react";

export default function PaymentChecker() {

    const [showIntro, setShowIntro] = useState(true);
    useEffect(() => {
        const timer = setTimeout(() => setShowIntro(false), 2500);
        return () => clearTimeout(timer);
    }, []);

    
    return (
<>
    <div className="header-wrapper">
        <Header />
    </div>

    <div className="main-block-wrapper p-8 mx-0 space-y-8 text-center pb-12 relative flex">
        {showIntro && (
            <div className="flex items-center justify-center m-auto h-auto ">
              <p className="text-xl text-color-primary animate-pulse">
                Plata se proceseaza...
              </p>
            </div>
          )}
    </div>

    <div className="footer-wrapper" >
        <Footer />
    </div>
</>
    );
}