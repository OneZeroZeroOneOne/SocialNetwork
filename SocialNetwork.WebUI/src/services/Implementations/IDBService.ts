import { openDB, deleteDB, wrap, unwrap, IDBPDatabase, DBSchema, IDBPTransaction, IDBPObjectStore } from 'idb';
import { MyDB } from './IDBServiceSchema';
import { IPost } from '@/models/responses/PostViewModel';
import { IComment } from '@/models/responses/CommentViewModel';

let dbName = 'CommentsPosts';
let dbVersion = 1;

class IDBSrvice {
    private static dbInstance: IDBSrvice;

    public db: IDBPDatabase<MyDB>;

    private static async New(): Promise<IDBSrvice> {
        let db = new IDBSrvice()
        await db.Connect()
        return db;
    }

    public static async GetDb(): Promise<IDBSrvice> {
        if (IDBSrvice.dbInstance === undefined)
            IDBSrvice.dbInstance = await IDBSrvice.New();
        
        return IDBSrvice.dbInstance;
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

export default IDBSrvice;