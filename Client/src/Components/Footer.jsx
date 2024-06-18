import { Link } from 'react-router-dom';
import Contacts from './Contacts';
import Signature from './Signature';

export default function Footer() {
    return (
        <div className='footer'>
            <div className='footer-top'>
                <Link to='/'><img className='footer-logo' src='/deltsz.ico' alt='logo' /></Link>
                <Contacts />
            </div>
            <Signature />
        </div>
    );
}