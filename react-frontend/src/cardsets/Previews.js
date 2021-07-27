import React, { useEffect, useState } from 'react';

import DataHandler from '../common/DataHandler';
import FetchJson from '../common/FetchJson';
import Preview from './Preview';
import styles from './Previews.module.css';
import { useParams } from "react-router-dom";

export default function Previews(){
  const [error, setError] = useState(false);
  const [clusters, setClusters] = useState(null);
  const { cardset } = useParams();

  useEffect(() => {
    async function fetchClusters(){
      const json = await FetchJson(`/api/clusters/${cardset}`);
      if(json.length > 0){
        setClusters(json);
      } else {
        console.error(`No clusters were found for cardset ${cardset}.`);
        setError(true);
      }
    }
    fetchClusters();
  }, [cardset]);

  const render = () => {
    return(
      <div className={styles['previews']}>
        <h1>{cardset.charAt(0).toUpperCase() + cardset.slice(1)}</h1>
        <div className={styles['preview-grid']}>
          {clusters.map((cluster) => <Preview key={cluster.id} id={cluster.id} top={cluster.top_cards.slice(0,3)} />)}
        </div>
      </div>
    );
  }

  return <DataHandler error={error} data={clusters} render={render}/>;
}