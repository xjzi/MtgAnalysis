import React, { useEffect, useState } from 'react';

import styles from './Card.module.css';

export default function Card({ card, frequency=null }) {
  const [url, setUrl] = useState(null);

  useEffect(() => {
    async function fetchLink(){
      const baseUrl = new URL("https://scryfall.com/search");
      const param = `!"${card}"`;
      baseUrl.searchParams.append('q', param);
      setUrl(baseUrl);
    }
    fetchLink();
  }, [card]);

  const line = frequency ? `${frequency} ${card}` : card;

  return <a href={url} className={styles['card']}>{line}</a>;
}