import React, { useEffect, useState } from 'react';

function Card({ card }) {
  const [isShown, setIsShown] = useState(false);
  const [url, setUrl] = useState('');

  useEffect(() => {
    const url = new URL("https://scryfall.com/search");
    const search = '!' + card;
    url.searchParams.append('q', search.replaceAll(' ', ''));
    setUrl(url);
    console.log('set url');
  }, [card])

  return (
    <div>
      <a
        href={url}
        className="App-link"
        target="_blank"
        rel="noopener noreferrer"
        onMouseEnter={() => setIsShown(true)}
        onMouseLeave={() => setIsShown(false)}
      >{card}</a>
      {isShown && (
        <dialog open style={{ position: 'fixed'}}>
          I'll appear when you hover over the button.
        </dialog>
      )}
    </div>
  );
}

export default Card;