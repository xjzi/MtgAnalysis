import styles from './About.module.css';
import summary from './Summary';

export default function About(){
    return <div className={styles['about']}>{summary}</div>;
}