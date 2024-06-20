import { useState, useEffect } from 'react';
import useIntersection from './useIntersection';

export default function Counter({ start, end, duration }) {
    const [count, setCount] = useState(start);
    const [ref, isIntersecting] = useIntersection({
        threshold: 0.1
    });

    useEffect(() => {
        if (!isIntersecting) return;

        const startTimestamp = Date.now();
        const endTimestamp = startTimestamp + duration;

        const updateCount = () => {
            const now = Date.now();
            const progress = Math.min((now - startTimestamp) / duration, 1);
            const currentCount = Math.floor(progress * (end - start) + start);
            setCount(currentCount);

            if (now < endTimestamp) {
                requestAnimationFrame(updateCount);
            }
        };

        requestAnimationFrame(updateCount);
    }, [isIntersecting, start, end, duration]);

    return <h1 ref={ref}>{count > 9999 ? count.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ') : count}{end === 96 ? '%' : ''}</h1>;
}