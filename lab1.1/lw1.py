import numpy as np


def decompose_to_LU(a):
    # create emtpy LU-matrix
    lu_matrix = np.matrix(np.zeros([a.shape[0], a.shape[1]]))
    n = a.shape[0]

    for k in range(n):
        # calculate all residual k-row elements
        for j in range(k, n):
            lu_matrix[k, j] = a[k, j] - lu_matrix[k, :k] * lu_matrix[:k, j]
        # calculate all residual k-column elemetns
        for i in range(k + 1, n):
            lu_matrix[i, k] = (a[i, k] - lu_matrix[i, : k] * lu_matrix[: k, k]) / lu_matrix[k, k]

    return lu_matrix


def get_L(m):
    L = m.copy()
    for i in range(L.shape[0]):
        L[i, i] = 1
        L[i, i + 1:] = 0
    return np.matrix(L)


def get_U(m):
    U = m.copy()
    for i in range(1, U.shape[0]):
        U[i, :i] = 0
    return U


def solve_LU(lu_matrix, b):
    # get supporting vector y
    y = np.matrix(np.zeros([lu_matrix.shape[0], 1]))
    for i in range(y.shape[0]):
        y[i, 0] = b[i] - lu_matrix[i, :i] * y[:i]

    # get vector of answers x
    x = np.matrix(np.zeros([lu_matrix.shape[0], 1]))
    for i in range(1, x.shape[0] + 1):
        x[-i, 0] = (y[-i] - lu_matrix[-i, -i:] * x[-i:, 0]) / lu_matrix[-i, -i]
    # print("РЕШЕНИЕ СИСТЕМЫ СНИЗУ")
    # print(x)
    return x


def Det(U):
    det = 1
    for i in range(U.shape[0]):
        det = U[i, i] * det
    return det


def ChangeCol(two, one, i):
    for j in range(two.shape[0]):
        two[j, i] = one[j]
    return two


def InsertCol(two, one, i):
    for j in range(two.shape[0]):
        one[j] = two[j, i]
    return one


def Reverse(M, N):
    revl = np.matrix(np.zeros([M.shape[0], M.shape[0]]))
    rev = np.matrix(np.zeros([M.shape[0], M.shape[0]]))
    b = [0] * M.shape[0]
    s1 = []
    for i in range(M.shape[0]):
        b[i] = [0] * 1
    for i in range(M.shape[0]):
        b[i] = 1
        s1 = solve_LU(M, b)
        revl = ChangeCol(revl, s1, i)
        b[i] = 0
    for i in range(M.shape[0]):
        b = InsertCol(revl, b, i)
        s1 = solve_LU(N, b)
        rev = ChangeCol(rev, s1, i)
    return rev


if __name__ == '__main__':
    file = open("lw1_input.txt", "r")
    data = np.genfromtxt('lw1_input.TXT', delimiter=',')
    b = [0] * data.shape[0]
    a = np.matrix(np.zeros([data.shape[0], data.shape[0]]))
    for i in range(data.shape[0]):
        b[i] = [0] * 1
        b[i] = data[i, data.shape[0]]
    # data = [map(float, line.split("\t")) for line in file]
    # print(data[2, 1])
    for i in range(data.shape[0]):
        for j in range(data.shape[1] - 1):
            a[i, j] = data[i, j]
    print("ВХОДНЫЕ ДАННЫЕ")
    print(data)
    print('\n')
    #print(a)
    #print(b)
    LU = decompose_to_LU(a)

    L = get_L(LU)
    U = get_U(LU)

    okay = solve_LU(LU, b)
    print("ОПРЕДЕЛИТЕЛЬ СНИЗУ")
    print(Det(U))
    print('\n')
    print("ОБРАТНАЯ МАТРИЦА СНИЗУ")
    print(Reverse(L, U))
    np.savetxt('example_1.txt', okay)
