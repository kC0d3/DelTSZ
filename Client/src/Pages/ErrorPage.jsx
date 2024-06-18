import Footer from '../Components/Footer';
import Header from '../Components/Header';
import Body from '../Components/ErrorBody';

export default function ErrorPage({ loggedUser, setLoggedUser, bgImagesAmount }) {
  return (
    <div className='errorpage'>
      <Header {...{ loggedUser, setLoggedUser, bgImagesAmount }} />
      <Body />
      <Footer />
    </div>
  );
}
