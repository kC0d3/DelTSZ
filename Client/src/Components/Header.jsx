import { useLocation } from 'react-router-dom';
import NavBar from './NavBar';
import Slider from './Slider';

export default function Header({ loggedUser, setLoggedUser, showLogin, setShowLogin, slideData }) {
    const location = useLocation();

    return (
        <div className={`header ${location.pathname === '/' ? 'homepage' : ''}`}>
            {location.pathname === '/' ?
                <Slider slides={slideData.slides} slidesInterval={slideData.slidesinterval} /> :
                <img className='header-bg'
                    src={`/images/backgrounds/bg${Math.floor(Math.random() * slideData.bgimagesamount + 1)}.jpg`}
                    alt={`bg${Math.floor(Math.random() * slideData.bgimagesamount + 1)}.jpg`} />}
            <NavBar {...{ loggedUser, setLoggedUser, showLogin, setShowLogin }} />
        </div>
    );
}