import { useLocation } from 'react-router-dom';
import NavBar from './NavBar';
import Slider from './Slider';

export default function Header({ loggedUser, setLoggedUser, slides, interval, bgImagesAmount }) {
    const location = useLocation();

    return (
        <div className={`header ${location.pathname === '/' ? 'homepage' : ''}`}>
            {location.pathname === '/' ?
                <Slider {... { slides, interval }} /> :
                <img className='header-bg'
                    src={`/images/bg${Math.floor(Math.random() * bgImagesAmount + 1)}.jpg`}
                    alt={`bg${Math.floor(Math.random() * bgImagesAmount + 1)}.jpg`} />}
            <NavBar {...{ loggedUser, setLoggedUser }} />
        </div>
    );
}