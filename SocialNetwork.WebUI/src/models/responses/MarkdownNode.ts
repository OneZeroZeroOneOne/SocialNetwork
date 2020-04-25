export interface IMarkdownNode {
    node_id: number;
    parent_id: number;
    node: string;
    tag: string;
    text: string;
    attr: any;
    child: IMarkdownNode[]
}
