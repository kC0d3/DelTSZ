import { useNavigate } from 'react-router-dom';

export default function ErrorBody() {
    const navigate = useNavigate();

    return (<div className='errorpage-body'>
        <h1>404</h1>
        <h2>Page Not Found</h2>
        <h2>This page does not exist.</h2>
        <button className='errorpage-homebutton' onClick={() => navigate('/')}>Home</button>
    </div>
    );
}