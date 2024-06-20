import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

export default function Slider({ slides, slidesInterval }) {
    const [currentIndex, setCurrentIndex] = useState(0);
    const [previousIndex, setPreviousIndex] = useState(null);

    useEffect(() => {
        const changeSlide = () => {
            setPreviousIndex(currentIndex);
            setCurrentIndex((prevIndex) => (prevIndex + 1) % slides.length);
        }

        const intervalId = setInterval(changeSlide, slidesInterval);

        return () => clearInterval(intervalId);
    }, [slides.length, slidesInterval, currentIndex]);

    return (
        <div className='slider'>
            {slides.map((slide, index) => {
                let className = 'slide';
                if (index === currentIndex) {
                    className += ' active';
                } else if (index === previousIndex) {
                    className += ' previous';
                } else {
                    className += ' next';
                }

                return (
                    <div key={index} className={className}>
                        <img src={slide.img} alt="" />
                        <div className='slide-bubble'>
                            <h1>{slide.bubble.h1}</h1>
                            <p>{slide.bubble.p1}</p>
                            <p>{slide.bubble.p2}</p>
                            <Link to='/products/types'>
                                <button>{slide.bubble.button}</button>
                            </Link>
                        </div>
                    </div>
                );
            })}
        </div>
    );
}