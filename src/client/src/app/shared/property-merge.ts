/**
 * Mark: 继承时忽略指定成员
 * 被忽略的成员可以修改基类同名成员的类型
 */
type Omit<T, K extends keyof T> = Pick<T, Exclude<keyof T, K>>
/**
 * Mark: 继承时忽略指定成员并添加、合并新的成员
 */
type Merge<M, N> = Omit<M, Extract<keyof M, keyof N>> & N;