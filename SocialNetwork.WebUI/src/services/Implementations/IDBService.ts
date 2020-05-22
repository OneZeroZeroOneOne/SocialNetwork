import { openDB, deleteDB, wrap, unwrap, IDBPDatabase, DBSchema, IDBPTransaction, IDBPObjectStore } from 'idb';
import { MyDB } from '../../types/IDBServiceSchema';
import { IPost } from '@/models/responses/PostViewModel';
import { IComment } from '@/models/responses/CommentViewModel';
import { IIDBService } from '../Abstractions/IIDBService';

let dbName = 'CommentsPosts';
let dbVersion = 1;

class IDBService implements IIDBService {
    public db: IDBPDatabase<MyDB>;

    private static async New(): Promise<IIDBService> {
        let db = new IDBService()
        await db.Connect()
        return db;
    }

    public async Connect(): Promise<void> {
        let self = this;
        this.db = await openDB<MyDB>(dbName, dbVersion, {
            upgrade(db, oldVersion, newVersion, transaction) {
                self.onUpgrade(db, oldVersion, newVersion, transaction);
            },
            blocked() {
                self.onBlocked();
            },
            blocking() {
                self.onBlocking();
            },
            terminated() {
                self.onTerminated();
            },
        })
    }

    public async getComment(comment_id: number): Promise<IComment|undefined> {
        return await this.db.get("comments", comment_id);
    }

    public async getPost(post_id: number): Promise<IPost|undefined> {
        return await this.db.get("posts", post_id);
    }

    public async addPosts(posts: IPost[]): Promise<void> {
        let promises: any[] = []
        for (let index = 0; index < posts.length; index++) {
            promises.push(this.db.put('posts', posts[index]))
        }

        await Promise.all(promises);
    }

    public async addComments(comments: IComment[]): Promise<void> {
        let promises: any[] = []
        for (let index = 0; index < comments.length; index++) {
            promises.push(this.db.put('comments', comments[index]))
        }

        await Promise.all(promises);
    }

    private onUpgrade(database: IDBPDatabase<MyDB>, oldVersion: number, newVersion: number | null, transaction: IDBPTransaction<MyDB>) {
        let comments = database.createObjectStore('comments', {
            keyPath: 'id'
        })
        let posts = database.createObjectStore('posts', {
            keyPath: 'id'
        })
    }

    private onBlocked() {

    }

    private onBlocking() {

    }

    private onTerminated() {

    }
}

export default new IDBService();