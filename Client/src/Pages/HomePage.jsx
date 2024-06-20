import Footer from '../Components/Footer';
import Header from '../Components/Header';
import Body from '../Components/HomeBody';

export default function HomePage({ loggedUser, setLoggedUser, slides, slidesInterval, achievementCounters, achievementDuration, achievementStart }) {

    return (
        <div className="homepage">
            <Header {...{ loggedUser, setLoggedUser, slides, slidesInterval }} />
            <Body {...{ achievementCounters, achievementDuration, achievementStart }} />
            <Footer />
        </div>
    );
}