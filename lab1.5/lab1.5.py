import copy
import numpy as np
from numpy.linalg import norm, eig


eps = 0.01

class Vector:
    def __init__(self, sz=None):
        if sz is not None:
            self.data = [0] * sz
        else:
            self.data = []

    def __getitem__(self, idx):
        return self.data[idx]

    def __setitem__(self, idx, item):
        if idx >= len(self):
            self.data.extend(idx + 1)
        self.data[idx] = item

    def __len__(self):
        return len(self.data)

    def __str__(self):
        res = '\n'
        for i in self:
            res += '{0}\n'.format(i)
        return res

    def __add__(self, other):
        if isinstance(other, Matrix):
            other_vec = sum(other.data, [])
            added = [i + j for i, j in zip(self, other_vec)]
        elif isinstance(other, Vector):
            added = [i + j for i, j in zip(self, other.data)]
        return Vector.from_list(added)

    def __sub__(self, other):
        subbed = [i - j for i, j in zip(self, other)]
        return Vector.from_list(subbed)

    def append(self, item):
        self.data.append(item)

    def extend(self, item):
        self.data.extend(item)

    def get_data(self):
        return [round(i, 4) for i in self.data]

    def norm(self):
        return max([abs(i) for i in self])

    @classmethod
    def from_list(cls, list_):
        vec = Vector()
        vec.data = list_
        return vec

    @classmethod
    def copy(cls, orig):
        vec = Vector()
        vec.data = copy.deepcopy(orig.data)
        return vec


class Matrix:
    def __init__(self, orig=None):
        if orig is None:
            self.non_copy_constructor()
        else:
            self.copy_constructor(orig)

    def non_copy_constructor(self):
        self.data = []

    def copy_constructor(self, orig):
        self.data = copy.deepcopy(orig.data)

    def __getitem__(self, idx):
        return self.data[idx]

    def __setitem__(self, idx, item):
        if idx >= len(self):
            self.data.extend(idx + 1)
        self.data[idx] = item

    def __len__(self):
        return len(self.data)

    def __str__(self):
        res = '\n'
        for i in range(len(self)):
            for j in range(len(self)):
                res += str(self.data[i][j]) + ' '
            res += '\n'
        return res

    def __sub__(self, other):
        subbed = [[self.data[i][j] - other.data[i][j] for j in range(sz)] for i in range(sz)]
        return Matrix.from_list(subbed)

    def get_data(self):
        return [[round(j, 4) for j in i] for i in self.data]

    def multiply(self, other):
        if isinstance(other, Vector):
            b = other.data
            result = Vector.from_list([sum(ea * eb for ea, eb in zip(a, b)) for a in self])
        elif isinstance(other, Matrix):
            other_T = list(zip(*other))
            result = Matrix.from_list([[sum(ea * eb for ea, eb in zip(a, b)) for b in other_T] for a in self])
        return result

    def debug_print(self):
        for i in self.data:
            print(*i)

    def transpose(self):
        self.data = [list(i) for i in zip(*self.data)]

    def diag(self):
        v = Vector.from_list([self.data[i][i] for i in range(len(self))])
        return v

    def get_column(self, idx):
        return Vector.from_list([self.data[i][idx] for i in range(len(self))])

    def norm(self):
        return max([sum(list(map(abs, self.data[i]))) for i in range(len(self))])

    @classmethod
    def _make_matrix(cls, rows):
        mat = Matrix()
        mat.data = rows
        return mat

    @classmethod
    def zero(cls, sz):
        obj = Matrix()
        obj.data = [[0] * sz for _ in range(sz)]
        return obj

    @classmethod
    def identity(cls, sz):
        obj = Matrix()
        obj.data = [[1 if i == j else 0 for j in range(sz)] for i in range(sz)]
        return obj

    @classmethod
    def from_list(cls, list_of_lists):
        rows = list_of_lists[:]
        return cls._make_matrix(rows)

    @classmethod
    def triangular_from_matrix(cls, orig, type_tril=None):
        obj = Matrix(orig)
        if type_tril == 'lower':
            for i in range(len(orig)):
                for j in range(i, len(orig)):
                    obj[i][j] = 0
        elif type_tril == 'upper':
            for i in range(len(orig)):
                for j in range(0, i):
                    obj[i][j] = 0
        return obj



def numpy_eig(matrix, my_values):
    print("My eigenvalues:")
    print(my_values)

    a = np.array(matrix.get_data())
    eig_np = eig(a)
    print("Numpy eigenvalues:")
    print(eig_np[0].round(3))


def sign(x):
    return -1 if x < 0 else 1 if x > 0 else 0


def householder(a, sz, k):
    v = np.zeros(sz)
    a = np.array(a.get_data())
    v[k] = a[k] + sign(a[k]) * norm(a[k:])
    for i in range(k + 1, sz):
        v[i] = a[i]
    v = v[:, np.newaxis]
    H = np.eye(sz) - (2 / (v.T @ v)) * (v @ v.T)
    return Matrix.from_list(H.tolist())


def get_QR(A):
    sz = len(A)
    Q = Matrix.identity(sz)
    A_i = Matrix(A)

    for i in range(sz - 1):
        col = A_i.get_column(i)
        H = householder(col, len(A_i), i)
        Q = Q.multiply(H)
        A_i = H.multiply(A_i)

    return Q, A_i


def get_roots(A, i):
    sz = len(A)
    a11 = A[i][i]
    a12 = A[i][i + 1] if i + 1 < sz else 0
    a21 = A[i + 1][i] if i + 1 < sz else 0
    a22 = A[i + 1][i + 1] if i + 1 < sz else 0
    return np.roots((1, -a11 - a22, a11 * a22 - a12 * a21))


def finish_iter_for_complex(A, eps, i):
    Q, R = get_QR(A)
    A_next = R.multiply(Q)
    lambda1 = get_roots(A, i)
    lambda2 = get_roots(A_next, i)
    return True if abs(lambda1[0] - lambda2[0]) <= eps and \
                   abs(lambda1[1] - lambda2[1]) <= eps else False


def get_eigenvalue(A, eps, i):
    A_i = Matrix(A)
    while True:
        Q, R = get_QR(A_i)
        A_i = R.multiply(Q)
        a = np.array(A_i.get_data())
        if norm(a[i + 1:, i]) <= eps:
            res = (a[i][i], False, A_i)
            break
        elif norm(a[i + 2:, i]) <= eps and finish_iter_for_complex(A_i, eps, i):
            res = (get_roots(A_i, i), True, A_i)
            break
    return res


def QR_method(A, eps):
    res = Vector()
    i = 0
    A_i = Matrix(A)
    while i < len(A):
        eigenval = get_eigenvalue(A_i, eps, i)
        if eigenval[1]:
            res.extend(eigenval[0])
            i += 2
        else:
            res.append(eigenval[0])
            i += 1
        A_i = eigenval[2]
    return res, i


if __name__ == '__main__':

    data = []
    with open('C://Users//Никита//Desktop//числаки//lab1.4//1.4.txt') as m:
        for line in m:
            data.append(list(map(int, line.split())))

    A = Matrix()
    A.data = data

    print("epsilon = {}\n".format(eps))
    tmp, count_iter = QR_method(A, eps)
    numpy_eig(A, tmp)
