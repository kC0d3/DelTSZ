import Counter from './Counter';

export default function HomeBody({ achievementCounters, achievementDuration, achievementStart }) {
    return (
        <div className='homepage-body'>
            <div className='homepage-body-top'>
                <h3>DelTSZ in numbers.</h3>
                <h1>Which how much.</h1>
            </div>
            <div className='counter-body'>
                {achievementCounters.map((counter, index) => (
                    <div key={index} className='counter-card'>
                        <Counter start={achievementStart} end={counter.amount} duration={achievementDuration} />
                        <h3>{counter.achievement}</h3>
                        <p>{counter.description}</p>
                    </div>
                ))}
            </div>
        </div>
    );
}