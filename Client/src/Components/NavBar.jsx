import { Link } from 'react-router-dom';
import ProfileMenu from './ProfileMenu';

export default function NavBar({ loggedUser, setLoggedUser, setShowLogin }) {

    const handleShowLogin = () => {
        setShowLogin(true);
    }

    return (
        <div className='navbar'>
            <Link to='/' className='logo-link' reloadDocument={true}><img className='navbar-logo' src='/deltsz.ico' alt='logo' /></Link>
            <div className='navbar-links'>
                <Link className='about-us-link' to='/about-us'>About us</Link>
                <Link className='our-products-link' to='/our-products'>Our Products</Link>
                {loggedUser ?
                    <ProfileMenu {...{ loggedUser, setLoggedUser }} />
                    :
                    <Link className='login-link' onClick={handleShowLogin}>Login</Link>
                }
            </div>
        </div>
    );
}